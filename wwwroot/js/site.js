// Función para mostrar el overlay de carga
function showLoading() {
    document.getElementById('loadingOverlay').classList.add('active');
}

// Función para ocultar el overlay de carga
function hideLoading() {
    document.getElementById('loadingOverlay').classList.remove('active');
}

// Simula la carga de contenido
document.addEventListener('DOMContentLoaded', function () {
    showLoading();
    setTimeout(hideLoading, 300); // Oculta el loading después de 2 segundos
});