// api-service.js - Servicio principal para consumir todas las APIs

// ==========================================
// FUNCIONES DE UTILIDAD
// ==========================================

// Mostrar notificación toast
function mostrarNotificacion(mensaje, tipo = 'success') {
    const toast = document.createElement('div');
    toast.className = `toast toast-${tipo}`;
    toast.innerHTML = `
        <div class="toast-content">
            <span class="toast-icon">${tipo === 'success' ? '✓' : '⚠'}</span>
            <span class="toast-mensaje">${mensaje}</span>
        </div>
    `;
    
    document.body.appendChild(toast);
    
    setTimeout(() => {
        toast.classList.add('show');
    }, 100);
    
    setTimeout(() => {
        toast.classList.remove('show');
        setTimeout(() => toast.remove(), 300);
    }, 3000);
}

// ==========================================
// API DE CLIENTES
// ==========================================

const ClientesAPI = {
    // Obtener todos los clientes
    async obtenerTodos() {
        try {
            const response = await fetch('/api/ClientesApi');
            return await response.json();
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al cargar clientes', 'error');
        }
    },

    // Obtener un cliente
    async obtenerPorId(id) {
        try {
            const response = await fetch(`/api/ClientesApi/${id}`);
            if (!response.ok) throw new Error('Cliente no encontrado');
            return await response.json();
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Cliente no encontrado', 'error');
        }
    },

    // Crear cliente
    async crear(cliente) {
        try {
            const response = await fetch('/api/ClientesApi', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(cliente)
            });
            if (!response.ok) throw new Error('Error al crear');
            const resultado = await response.json();
            mostrarNotificacion('Cliente creado exitosamente');
            return resultado;
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al crear cliente', 'error');
        }
    },

    // Actualizar cliente
    async actualizar(id, cliente) {
        try {
            const response = await fetch(`/api/ClientesApi/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(cliente)
            });
            if (!response.ok) throw new Error('Error al actualizar');
            const resultado = await response.json();
            mostrarNotificacion('Cliente actualizado exitosamente');
            return resultado;
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al actualizar cliente', 'error');
        }
    },

    // Eliminar cliente
    async eliminar(id) {
        try {
            const response = await fetch(`/api/ClientesApi/${id}`, {
                method: 'DELETE'
            });
            if (!response.ok) throw new Error('Error al eliminar');
            mostrarNotificacion('Cliente eliminado exitosamente');
            return await response.json();
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al eliminar cliente', 'error');
        }
    },

    // Buscar clientes
    async buscar(nombre) {
        try {
            const response = await fetch(`/api/ClientesApi/buscar?nombre=${encodeURIComponent(nombre)}`);
            return await response.json();
        } catch (error) {
            console.error('Error:', error);
            return [];
        }
    }
};

// ==========================================
// API DE CITAS
// ==========================================

const CitasAPI = {
    // Obtener todas las citas
    async obtenerTodas() {
        try {
            const response = await fetch('/api/CitasApi');
            return await response.json();
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al cargar citas', 'error');
        }
    },

    // Obtener citas de hoy
    async obtenerHoy() {
        try {
            const response = await fetch('/api/CitasApi/hoy');
            return await response.json();
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al cargar citas de hoy', 'error');
        }
    },

    // Crear cita
    async crear(cita) {
        try {
            const response = await fetch('/api/CitasApi', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(cita)
            });
            if (!response.ok) throw new Error('Error al crear');
            const resultado = await response.json();
            mostrarNotificacion('Cita creada exitosamente');
            return resultado;
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al crear cita', 'error');
        }
    },

    // Completar cita
    async completar(id) {
        try {
            const response = await fetch(`/api/CitasApi/${id}/completar`, {
                method: 'PUT'
            });
            if (!response.ok) throw new Error('Error al completar');
            const resultado = await response.json();
            mostrarNotificacion('Cita completada');
            return resultado;
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al completar cita', 'error');
        }
    },

    // Eliminar cita
    async eliminar(id) {
        try {
            const response = await fetch(`/api/CitasApi/${id}`, {
                method: 'DELETE'
            });
            if (!response.ok) throw new Error('Error al eliminar');
            mostrarNotificacion('Cita eliminada exitosamente');
            return await response.json();
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al eliminar cita', 'error');
        }
    }
};

// ==========================================
// API DE SERVICIOS
// ==========================================

const ServiciosAPI = {
    // Obtener todos los servicios
    async obtenerTodos() {
        try {
            const response = await fetch('/api/ServiciosApi');
            return await response.json();
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al cargar servicios', 'error');
        }
    },

    // Obtener servicios por categoría
    async obtenerPorCategoria(categoria) {
        try {
            const response = await fetch(`/api/ServiciosApi/categoria/${categoria}`);
            return await response.json();
        } catch (error) {
            console.error('Error:', error);
            return [];
        }
    },

    // Crear servicio
    async crear(servicio) {
        try {
            const response = await fetch('/api/ServiciosApi', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(servicio)
            });
            if (!response.ok) throw new Error('Error al crear');
            const resultado = await response.json();
            mostrarNotificacion('Servicio creado exitosamente');
            return resultado;
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al crear servicio', 'error');
        }
    },

    // Eliminar servicio
    async eliminar(id) {
        try {
            const response = await fetch(`/api/ServiciosApi/${id}`, {
                method: 'DELETE'
            });
            if (!response.ok) throw new Error('Error al eliminar');
            mostrarNotificacion('Servicio eliminado exitosamente');
            return await response.json();
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al eliminar servicio', 'error');
        }
    }
};

// ==========================================
// API DE PRODUCTOS
// ==========================================

const ProductosAPI = {
    // Obtener todos los productos
    async obtenerTodos() {
        try {
            const response = await fetch('/api/ProductosApi');
            return await response.json();
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al cargar productos', 'error');
        }
    },

    // Obtener productos con stock bajo
    async obtenerStockBajo() {
        try {
            const response = await fetch('/api/ProductosApi/stock-bajo');
            return await response.json();
        } catch (error) {
            console.error('Error:', error);
            return [];
        }
    },

    // Crear producto
    async crear(producto) {
        try {
            const response = await fetch('/api/ProductosApi', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(producto)
            });
            if (!response.ok) throw new Error('Error al crear');
            const resultado = await response.json();
            mostrarNotificacion('Producto creado exitosamente');
            return resultado;
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al crear producto', 'error');
        }
    },

    // Actualizar stock
    async actualizarStock(id, cantidad) {
        try {
            const response = await fetch(`/api/ProductosApi/${id}/actualizar-stock`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(cantidad)
            });
            if (!response.ok) throw new Error('Error al actualizar');
            const resultado = await response.json();
            mostrarNotificacion('Stock actualizado');
            return resultado;
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al actualizar stock', 'error');
        }
    },

    // Eliminar producto
    async eliminar(id) {
        try {
            const response = await fetch(`/api/ProductosApi/${id}`, {
                method: 'DELETE'
            });
            if (!response.ok) throw new Error('Error al eliminar');
            mostrarNotificacion('Producto eliminado exitosamente');
            return await response.json();
        } catch (error) {
            console.error('Error:', error);
            mostrarNotificacion('Error al eliminar producto', 'error');
        }
    }
};

// Exportar para usar en otros archivos
window.ClientesAPI = ClientesAPI;
window.CitasAPI = CitasAPI;
window.ServiciosAPI = ServiciosAPI;
window.ProductosAPI = ProductosAPI;
