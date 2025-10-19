window._env = {
    environment: 'QA', // Cambia esto según el entorno: 'Development' | 'QA' | 'Production'

    // Configuración por entorno
    API_URL: {
        Development: "http://localhost:8080",
        QA: "http://localhost:8080",
        Production: "https://api.opticasvistareal.com" // reemplazar cuando esté lista
    }
};

// ✅ Expone la URL activa como variable global simple
window.apiUrl = window._env.API_URL[window._env.environment];
