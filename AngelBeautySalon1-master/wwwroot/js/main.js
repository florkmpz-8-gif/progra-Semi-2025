// Función para mostrar la fecha actual
function updateCurrentDate() {
    const dateElement = document.getElementById('currentDate');
    if (!dateElement) return;
    
    const now = new Date();
    
    const options = { 
        weekday: 'long', 
        year: 'numeric', 
        month: 'long', 
        day: 'numeric' 
    };
    
    const dateString = now.toLocaleDateString('es-ES', options);
    
    // Capitalizar la primera letra
    const formattedDate = dateString.charAt(0).toUpperCase() + dateString.slice(1);
    
    dateElement.textContent = `Fecha actual: ${formattedDate}`;
}

// Ejecutar al cargar la página
document.addEventListener('DOMContentLoaded', function() {
    updateCurrentDate();
    
    // Agregar efectos de hover mejorados a las tarjetas
    const cards = document.querySelectorAll('.card');
    cards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-10px) scale(1.02)';
        });
        
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0) scale(1)';
        });
    });
});

// Actualizar la fecha cada minuto
setInterval(updateCurrentDate, 60000);
