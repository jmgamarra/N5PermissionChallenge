import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import PermisoForm from './PermisoForm'; // Asegúrate de importar el formulario

const PermisoEdit = () => {
  const { id } = useParams();
  const [initialValues, setInitialValues] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchPermiso = async () => {
      try {
        const response = await axios.get(`http://localhost:5001/api/permiso/${id}`);
        setInitialValues(response.data);
      } catch (error) {
        console.error('Error al cargar el permiso:', error);
      }
    };

    fetchPermiso();
  }, [id]);

  const handleEditPermiso = async (formValues) => {
    try {
      await axios.put(`http://localhost:5001/api/permiso/${id}`, formValues);
      alert('Permiso actualizado correctamente');
      navigate('/');
    } catch (error) {
      console.error('Error al editar el permiso:', error);
    }
  };
 
  return (
    initialValues && (
      <PermisoForm
        initialValues={initialValues}
        onSubmit={handleEditPermiso}
        isEditMode={true}
      />
    )
  );
};

export default PermisoEdit;
