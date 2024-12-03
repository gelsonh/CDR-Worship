// Object to store new comments by scheduleSongId
let newComments = {};

// Function to play a notification sound for new comments
function playNotificationSound() {
  const soundToggle = document.getElementById("soundToggle");
  if (soundToggle && soundToggle.checked) {
    const newCommentSound = new Audio("/sounds/new-comment.wav");
    newCommentSound.volume = 0.5;
    newCommentSound
      .play()
      .catch((error) => console.error("Error playing sound:", error));
  }
}

// Establish connection to SignalR hub for real-time comment notifications
const connection = new signalR.HubConnectionBuilder()
  .withUrl("/commentHub")
  .build();

connection
  .start()
  .then(() => console.log("Connected to the Comment Hub"))
  .catch((err) => console.error("Error connecting to SignalR:", err));

connection.on("ReceiveComment", function (commentData) {
  console.log("Comment received:", commentData);
  handleNewComment(commentData);
});

// Handle a new comment
function handleNewComment(commentData) {
  const scheduleSongId = commentData.scheduleSongId;

  // Add comment to newComments object
  if (!newComments[scheduleSongId]) {
    newComments[scheduleSongId] = {count: 0, comments: []};
  }
  newComments[scheduleSongId].count += 1;
  newComments[scheduleSongId].comments.push(commentData);

  // Save in localStorage and update UI
  localStorage.setItem("newComments", JSON.stringify(newComments));
  updateNotificationBadge();
  updateNotificationDropdown();

  playNotificationSound();
}

// Update notification badge for navbar and footer
function updateNotificationBadge() {
  const badges = [
    document.getElementById("notificationBadge"),
    document.getElementById("footerNotificationBadge"),
  ];
  const totalNewComments = Object.values(newComments).reduce(
    (acc, curr) => acc + curr.count,
    0
  );

  badges.forEach((badge) => {
    if (badge) {
      badge.style.display = totalNewComments > 0 ? "inline-block" : "none";
      badge.textContent = totalNewComments > 99 ? "99+" : totalNewComments;

      // Apply CSS class for consistent styling
      badge.classList.add("notification-badge");
    }
  });
}

// Update notification dropdown for navbar and footer
function updateNotificationDropdown() {
  const dropdowns = [
    document.getElementById("notificationItems"),
    document.getElementById("notificationItemsFooter"),
  ];
  const scheduleSongIds = Object.keys(newComments);
  const defaultMessage =
    '<li class="dropdown-item text-center">No hay nuevos comentarios.</li>';

  dropdowns.forEach((dropdown) => {
    if (!dropdown) return;

    if (scheduleSongIds.length === 0) {
      dropdown.innerHTML = defaultMessage;
      return;
    }

    let dropdownContent = "";
    scheduleSongIds.forEach((scheduleSongId) => {
      const schedule = newComments[scheduleSongId];
      const scheduleSongName =
        schedule.comments[0]?.name || "Canción desconocida";

      dropdownContent += `<li class="dropdown-header">Canción: ${scheduleSongName}</li>`;

      schedule.comments.forEach((comment) => {
        dropdownContent += `
          <li>
            <a class="dropdown-item" href="/ScheduledSongs/Details/${scheduleSongId}">
              ${comment.commentUserName}: ${truncateText(
          comment.commentText,
          50
        )}
            </a>
          </li>
        `;
      });

      dropdownContent += '<li><hr class="dropdown-divider"></li>';
    });

    dropdownContent = dropdownContent.replace(
      /<li><hr class="dropdown-divider"><\/li>$/,
      ""
    );
    dropdown.innerHTML = dropdownContent;
  });
}

// Truncate long text
function truncateText(text, maxLength) {
  return text.length > maxLength ? text.substring(0, maxLength) + "..." : text;
}

// Initialize notifications on page load
document.addEventListener("DOMContentLoaded", function () {
  const storedComments = localStorage.getItem("newComments");
  if (storedComments) {
    newComments = JSON.parse(storedComments);
  }

  updateNotificationBadge();
  updateNotificationDropdown();

  // Clear notifications if accessing specific schedule details
  const path = window.location.pathname;
  const match = path.match(/\/ScheduledSongs\/Details\/(\d+)/);
  if (match && match[1]) {
    const scheduleSongId = match[1];
    if (newComments[scheduleSongId]) {
      delete newComments[scheduleSongId];
      localStorage.setItem("newComments", JSON.stringify(newComments));
      updateNotificationBadge();
      updateNotificationDropdown();
    }
  }
});
