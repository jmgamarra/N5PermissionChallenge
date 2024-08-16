import React from 'react';
import ReactDOM from 'react-dom/client'; // Importar desde 'react-dom/client'
import App from './App';
import './index.css'; // Si tienes estilos globales

// Verificar si el elemento con el id 'root' existe
const rootElement = document.getElementById('root');

if (rootElement) {
  // Crear la raíz y renderizar la aplicación
  const root = ReactDOM.createRoot(rootElement);
  root.render(
    <React.StrictMode>
      <App />
    </React.StrictMode>
  );
} else {
  console.error('Element with id "root" not found.');
}
