import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { connectToWebSocket } from '../websocket';
import {
  Container,
  Grid,
  Paper,
  TextField,
  Button,
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Snackbar,
  Alert,
} from '@mui/material';

function OrdersPage() {
  const [orders, setOrders] = useState([]);
  const [newOrder, setNewOrder] = useState({ userID: '', bill: '' });
  const [notification, setNotification] = useState({ open: false, message: '', severity: 'success' });
  const [socket, setSocket] = useState(null);

  const API_URL = window.env?.REACT_APP_GATEWAY_SERVER || '';

  const fetchOrders = async () => {
    try {
      const response = await axios.get(`${API_URL}/api/orders`);
      setOrders(response.data.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt)));
    } catch (error) {
      console.error('Error fetching orders:', error);
    }
  };

  const handleCloseNotification = () => {
    setNotification({ ...notification, open: false });
  };

  const showNotification = (message, severity = 'error') => {
    setNotification({
      open: true,
      message,
      severity,
    });
  };

  const ensureWebSocketConnection = () => {
    if (!socket || socket.readyState === WebSocket.CLOSED) {
      const newSocket = connectToWebSocket(message => {
        const [orderId, status] = message.split(';');
        setOrders(prevOrders => prevOrders.map(order => 
          order.id === orderId 
            ? {...order, status} 
            : order
        ));
      });
      setSocket(newSocket);
      return newSocket;
    }
    return socket;
  };

  useEffect(() => {
    fetchOrders();
    return () => {
      if (socket) {
        socket.close();
      }
    };
  }, []);

  const handleCreateOrder = async (e) => {
    e.preventDefault();

    let newUserID = parseInt(newOrder.userID);
    if (isNaN(newUserID)) {
      showNotification('Некорректный ID счета', 'warning');
      return;
    }

    let newBill = parseFloat(newOrder.bill);
    if (isNaN(newBill)) {
      showNotification('Некорректная сумма', 'warning');
      return;
    }

    if (newBill <= 0) {
      showNotification('Сумма не может быть отрицательной или равной нулю', 'warning');
      return;
    }

    setNewOrder({ userID: '', bill: '' });

    ensureWebSocketConnection();

    try {
      const formData = new FormData();
      formData.append('userID', newUserID);
      formData.append('bill', newBill);

      const response = await axios.post(`${API_URL}/api/orders`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });

      const { id } = response.data;

      setOrders(prevOrders => [{
        id: id,
        userID: newUserID,
        bill: newBill,
        createdAt: new Date().toISOString(),
        status: 'newOrder'
      }, ...prevOrders]);

      showNotification('Заказ успешно создан', 'success');  
    } catch (error) {
      if (error.response?.data?.error) {
        showNotification(error.response.data.error);
      } else {
        showNotification('Не удалось создать заказ');
      }
      console.error('Error creating order:', error);
    }
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleString('ru-RU', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Grid container spacing={3}>
        {/* Create Order Form */}
        <Grid item xs={12} md={6}>
          <Paper sx={{ p: 2 }}>
            <Typography variant="h6" gutterBottom>
              Создать заказ
            </Typography>
            <form onSubmit={handleCreateOrder}>
              <TextField
                fullWidth
                label="ID счета"
                type="number"
                value={newOrder.userID}
                onChange={(e) => setNewOrder({ ...newOrder, userID: e.target.value })}
                margin="normal"
                required
              />
              <TextField
                fullWidth
                label="Сумма заказа"
                type="number"
                value={newOrder.bill}
                onChange={(e) => setNewOrder({ ...newOrder, bill: e.target.value })}
                margin="normal"
                required
              />
              <Button type="submit" variant="contained" color="primary" sx={{ mt: 2 }}>
                Создать
              </Button>
            </form>
          </Paper>
        </Grid>

        {/* Orders Table */}
        <Grid item xs={12}>
          <Paper sx={{ p: 2 }}>
            <Typography variant="h6" gutterBottom>
              Список заказов
            </Typography>
            <TableContainer>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>ID заказа</TableCell>
                    <TableCell>ID счета</TableCell>
                    <TableCell align="right">Сумма</TableCell>
                    <TableCell>Дата создания</TableCell>
                    <TableCell>Статус</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {orders.map((order) => (
                    <TableRow key={order.id}>
                      <TableCell>{order.id}</TableCell>
                      <TableCell>{order.userID}</TableCell>
                      <TableCell align="right">{order.bill.toFixed(2)}</TableCell>
                      <TableCell>{formatDate(order.createdAt)}</TableCell>
                      <TableCell>{order.status}</TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          </Paper>
        </Grid>
      </Grid>
      <Snackbar
        open={notification.open}
        autoHideDuration={6000}
        onClose={handleCloseNotification}
        anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
      >
        <Alert
          onClose={handleCloseNotification}
          severity={notification.severity}
          sx={{ width: '100%' }}
        >
          {notification.message}
        </Alert>
      </Snackbar>
    </Container>
  );
}

export default OrdersPage; 