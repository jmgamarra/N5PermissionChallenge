import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, TextField, FormControl, InputLabel, Select, MenuItem, FormHelperText, Container, Typography, Box } from '@mui/material';
const formatDateForInput = (date) => {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  };
const PermisoForm = ({ initialValues = {}, onSubmit,isEditMode }) => {
  const [nombreEmpleado, setNombreEmpleado] = useState(initialValues.nombreEmpleado || undefined);
  const [apellidoEmpleado, setApellidoEmpleado] = useState(initialValues.apellidoEmpleado || undefined);
  const [fechaPermiso, setFechaPermiso] = useState(initialValues.fechaPermiso ? formatDateForInput(new Date(initialValues.fechaPermiso)) : '');
  const [tipoPermisoId, setTipoPermisoId] = useState(parseInt(initialValues.tipoId) || undefined);
  const [errors, setErrors] = useState({});
  const navigate = useNavigate();


  const validateForm = () => {
    
    const newErrors = {};
    if (!nombreEmpleado) newErrors.nombreEmpleado = 'El nombre es obligatorio';
    if (!apellidoEmpleado) newErrors.apellidoEmpleado = 'El apellido es obligatorio';
    if (!fechaPermiso) newErrors.fechaPermiso = 'La fecha es obligatoria';
    if (!tipoPermisoId) newErrors.tipoPermisoId = 'El tipo de permiso es obligatorio';
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    if (validateForm()) {
      console.log('Formulario válido');
      onSubmit({
        nombreEmpleado,
        apellidoEmpleado,
        fechaPermiso,
        tipoId: tipoPermisoId
      });
  
    } else {
      console.log('Errores en el formulario:', errors);
    }
  };
  const handleCancel = () => {
    navigate('/');
  };
  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        {isEditMode ? 'Editar Permiso' : 'Agregar Permiso'}
      </Typography>
      <form onSubmit={handleSubmit} >
        <TextField
          label="Nombre"
          value={nombreEmpleado}
          onChange={(e) => setNombreEmpleado(e.target.value)}
          fullWidth
          margin="normal"
          error={!!errors.nombreEmpleado}
          helperText={errors.nombreEmpleado}
        />
        <TextField
          label="Apellido"
          value={apellidoEmpleado}
          onChange={(e) => setApellidoEmpleado(e.target.value)}
          fullWidth
          margin="normal"
          error={!!errors.apellidoEmpleado}
          helperText={errors.apellidoEmpleado}
        />
        <TextField
          label="Fecha"
          type="date"
          value={fechaPermiso}
          onChange={(e) => setFechaPermiso(e.target.value)}
          fullWidth
          margin="normal"
          InputLabelProps={{ shrink: true }}
          error={!!errors.fechaPermiso}
          helperText={errors.fechaPermiso}
        />
        <FormControl fullWidth margin="normal" error={!!errors.tipoPermisoId}>
          <InputLabel id="tipo-permiso-label">Tipo de Permiso</InputLabel>
          <Select
            labelId="tipo-permiso-label"
            value={tipoPermisoId}
            onChange={(e) => setTipoPermisoId(e.target.value)}
            label="Tipo de Permiso"
          >
            <MenuItem value={1}>Tipo 1</MenuItem>
            <MenuItem value={2}>Tipo 2</MenuItem>
          </Select>
          <FormHelperText>{errors.tipoPermisoId}</FormHelperText>
        </FormControl>
        <Box display="flex" justifyContent="flex-end" mt={2}>
          <Button type="submit" variant="contained" color="primary" style={{ marginRight: '10px' }}>
            {isEditMode ? 'Editar' : 'Agregar'}
          </Button>
          <Button type="button" variant="contained" color="secondary" onClick={(e) => {handleCancel(e.target.value)}}>
            Cancelar
          </Button>
        </Box>
      </form>
    </Container>
  );
};

export default PermisoForm;
