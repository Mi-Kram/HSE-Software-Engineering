import React from 'react';
import { Box, Typography, Container } from '@mui/material';
import ErrorOutlineIcon from '@mui/icons-material/ErrorOutline';

function NotFound() {
  return (
    <Container>
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          justifyContent: 'center',
          height: 'calc(100vh - 64px)',
          textAlign: 'center',
        }}
      >
        <ErrorOutlineIcon sx={{ fontSize: 150, color: 'error.main', mb: 4 }} />
        <Typography variant="h3" component="h1" gutterBottom color="error">
          404 Not Found
        </Typography>
        <Typography variant="h6" color="text.secondary">
          Запрашиваемая страница не существует
        </Typography>
      </Box>
    </Container>
  );
}

export default NotFound; 