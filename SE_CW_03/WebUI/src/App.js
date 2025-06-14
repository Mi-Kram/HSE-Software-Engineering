import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate, useLocation } from 'react-router-dom';
import { ThemeProvider, CssBaseline, Box, Toolbar } from '@mui/material';
import { createTheme } from '@mui/material/styles';
import HealthCheckGuard from "./components/HealthCheckGuard";
import NavigationBar from './components/NavigationBar';
import AccountsPage from './pages/AccountsPage';
import OrdersPage from './pages/OrdersPage';
import NotFound from './components/NotFound';

const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2',
    },
    secondary: {
      main: '#dc004e',
    },
  },
});

const validPaths = ['/', '/accounts', '/orders'];

function AppRoutes() {
  const location = useLocation();
  const isValidPath = validPaths.includes(location.pathname);

  if (!isValidPath) {
    return <NotFound />;
  }

  return (
    <HealthCheckGuard>
      <Routes>
        <Route path="/" element={<Navigate to="/accounts" replace />} />
        <Route path="/accounts" element={<AccountsPage />} />
        <Route path="/orders" element={<OrdersPage />} />
      </Routes>
    </HealthCheckGuard>
  );
}

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Router>
        <div style={{ display: 'flex', flexDirection: 'column' }}>
          <NavigationBar />
          <Toolbar />
          <Box component="main" sx={{ flexGrow: 1 }}>
            <Routes>
              <Route path="*" element={<AppRoutes />} />
            </Routes>
          </Box>
        </div>
      </Router>
    </ThemeProvider>
  );
}

export default App; 