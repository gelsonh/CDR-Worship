$(document).ready(function () {
    const rowCountSelect = $('#rowCount');
    const tableBody = $('#songTableBody');

    if (rowCountSelect.length && tableBody.length) {
        const allRows = tableBody.find('tr');

        function updateTable() {
            const rowCount = parseInt(rowCountSelect.val());
            allRows.each(function (index, row) {
                $(row).toggle(index < rowCount);
            });
        }

        rowCountSelect.on('change', updateTable);
        updateTable();
    }

    const searchInput = $('#searchInput');
    if (searchInput.length && tableBody.length) {
        searchInput.on('keyup', function () {
            const value = searchInput.val().toLowerCase();
            tableBody.find('tr').each(function () {
                const row = $(this);
                const rowText = row.text().toLowerCase();
                row.toggle(rowText.includes(value));
            });
        });
    }
});