import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Container, Typography, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';

const PermisoList = () => {
  const [permisos, setPermisos] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchPermisos = async () => {
      try {
        const response = await axios.get('http://localhost:5001/api/permiso');
        setPermisos(response.data.$values);
      } catch (error) {
        console.error('Error fetching permisos:', error);
      }
    };

    fetchPermisos();
  }, []);

  const handleEdit = (id) => {
    navigate(`/edit-permiso/${id}`);
  };

  const handleAdd = () => {
    navigate('/add-permiso');
  };

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Lista de Permisos
      </Typography>
      <Button variant="contained" color="primary" onClick={handleAdd} sx={{ mb: 2 }}>
        Agregar
      </Button>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#f5f5f5', color: '#333' }}>Nombre</TableCell>
              <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#f5f5f5', color: '#333' }}>Apellido</TableCell>
              <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#f5f5f5', color: '#333' }}>Fecha</TableCell>
              <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#f5f5f5', color: '#333' }}>Acciones</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {permisos.map((permiso) => (
              <TableRow key={permiso.id}>
                <TableCell>{permiso.nombreEmpleado}</TableCell>
                <TableCell>{permiso.apellidoEmpleado}</TableCell>
                <TableCell>{new Date(permiso.fechaPermiso).toLocaleDateString()}</TableCell>
                <TableCell>
                  <Button
                    variant="contained"
                    color="primary"
                    onClick={() => handleEdit(permiso.id)}
                  >
                    Editar
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  );
};

export default PermisoList;
