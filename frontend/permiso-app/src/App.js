// src/App.js
import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import PermisoAdd from './components/PermisoAdd';
import PermisoEdit from './components/PermisoEdit';
import PermisoList from './components/PermisoList';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<PermisoList />} />
        <Route path="/add-permiso" element={<PermisoAdd />} />
        <Route path="/edit-permiso/:id" element={<PermisoEdit />} />
      </Routes>
    </Router>
  );
}

export default App;
