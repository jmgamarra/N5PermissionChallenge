// src/components/PermisoForm.js
import React, { useState } from 'react';
import { TextField, Button, Container, Typography,FormControl,InputLabel,Select,MenuItem } from '@mui/material';
import axios from 'axios';

const PermisoForm = () => {
  const [nombreEmpleado, setNombreEmpleado] = useState('');
  const [apellidoEmpleado, setApellidoEmpleado] = useState('');
  const [fechaPermiso, setFechaPermiso] = useState('');
  const [tipoPermisoId, setTipoPermisoId] = useState('');

  // const handleSubmit = async (event) => {
  //   event.preventDefault();
  //   try {
  //     await axios.post('http://localhost:5001/api/permiso', {
  //       nombreEmpleado,
  //       apellidoEmpleado,
  //       fechaPermiso,
  //       tipoPermisoId,
  //     },{   headers: {
  //       'Content-Type': 'application/json'
  //   }});
  //     alert('Permiso enviado correctamente');
  //   } catch (error) {
  //     console.error('Hubo un error al enviar el permiso:', error);
  //   }
  // };
  const handleSubmit = async (event) => {
    event.preventDefault();
    try {
        const response = await axios.post('http://backend:8080/api/permiso', {
            nombre: 'Jorge',      // Asegúrate de que estos nombres coincidan con los que espera el backend
            apellido: 'Ape',
            fecha: '2024-08-14',          // Verifica el formato de fecha
            tipoId: 1         // Verifica que sea un número
        }, {
            headers: {
                'Content-Type': 'application/json'
            }
        });
        console.log(response); 
        alert('Permiso enviado correctamente');
    } catch (error) {
        console.error('Hubo un error al enviar el permiso:', error.response); // Muestra el detalle del error
    }
};
  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Registrar Permiso
      </Typography>
      <form onSubmit={handleSubmit}>
        <TextField
          label="Nombre del Empleado"
          variant="outlined"
          fullWidth
          margin="normal"
          value={nombreEmpleado}
          onChange={(e) => setNombreEmpleado(e.target.value)}
        />
        <TextField
          label="Apellido del Empleado"
          variant="outlined"
          fullWidth
          margin="normal"
          value={apellidoEmpleado}
          onChange={(e) => setApellidoEmpleado(e.target.value)}
        />
        <TextField
          label="Fecha del Permiso"
          type="date"
          variant="outlined"
          fullWidth
          margin="normal"
          InputLabelProps={{ shrink: true }}
          value={fechaPermiso}
          onChange={(e) => setFechaPermiso(e.target.value)}
        />
            <FormControl variant="outlined" fullWidth margin="normal">
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
    </FormControl>
        <Button type="submit" variant="contained" color="primary">
          Enviar
        </Button>
      </form>
    </Container>
  );
};

export default PermisoForm;
