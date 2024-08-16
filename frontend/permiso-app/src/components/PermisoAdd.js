import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import PermisoForm from './PermisoForm'; // Asegúrate de importar el formulario

const AddPermiso = () => {
  const [initialValues] = useState({
    nombreEmpleado: '',
    apellidoEmpleado: '',
    fechaPermiso: '',
    tipoId: ''
  });
  const navigate = useNavigate();

  const handleAddPermiso = async (formValues) => {
    
    try {
      await axios.post('http://localhost:5001/api/permiso', formValues);
     
      alert('Permiso agregado correctamente');
      navigate('/');
    } catch (error) {
      console.error('Error al agregar el permiso:', error);
    }
  };

  return (
    <PermisoForm
      initialValues={initialValues}
      onSubmit={handleAddPermiso}
      isEditMode={false}
    />
  );
};

export default AddPermiso;
