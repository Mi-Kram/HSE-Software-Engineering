import React from 'react';
import { AppBar, Tabs, Tab, Toolbar, Typography } from '@mui/material';
import { useNavigate, useLocation } from 'react-router-dom';

function NavigationBar() {
  const navigate = useNavigate();
  const location = useLocation();

  const handleChange = (event, newValue) => {
    navigate(newValue);
  };

  return (
    <AppBar position="fixed">
      <Toolbar>
        <Typography variant="h6" sx={{ marginRight: 4 }}>
          Orders & Payments
        </Typography>
        <Tabs 
          value={location.pathname} 
          onChange={handleChange}
          textColor="inherit"
          indicatorColor="secondary"
        >
          <Tab label="Счета" value="/accounts" />
          <Tab label="Заказы" value="/orders" />
        </Tabs>
      </Toolbar>
    </AppBar>
  );
}

export default NavigationBar; 