import React, { useState, useEffect } from 'react';
import axios from 'axios';
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

function AccountsPage() {
  const [accounts, setAccounts] = useState([]);
  const [newAccount, setNewAccount] = useState({ caption: '', initialBalance: '' });
  const [topUp, setTopUp] = useState({ userID: '', operation: '' });
  const [notification, setNotification] = useState({ open: false, message: '', severity: 'success' });

  const API_URL = window.env?.REACT_APP_GATEWAY_SERVER || '';

  const fetchAccounts = async () => {
    try {
      const response = await axios.get(`${API_URL}/api/accounts`);
      setAccounts(response.data.sort((a, b) => a.userID - b.userID));
    } catch (error) {
      console.error('Error fetching accounts:', error);
    }
  };

  useEffect(() => {
    fetchAccounts();
  }, []);

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

  const handleCreateAccount = async (e) => {
    e.preventDefault();

    let newCaption = newAccount.caption.trim();
    if (newCaption.length == 0) {
      showNotification('Название счета не может быть пустым', 'warning');
      return;
    }

    let newInitialBalance = parseFloat(newAccount.initialBalance);
    if (isNaN(newInitialBalance)) {
      showNotification('Некорректный начальный баланс', 'warning');
      return;
    }
    
    if (newInitialBalance < 0) {
      showNotification('Начальный баланс не может быть отрицательным', 'warning');
      return;
    }

    setNewAccount({ caption: '', initialBalance: '' });

    try {
      const formData = new FormData();
      formData.append('caption', newCaption);
      formData.append('initialBalance', newInitialBalance);

      const response = await axios.post(`${API_URL}/api/accounts`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });

      const { user_id } = response.data;
      
      setAccounts(prevAccounts => [...prevAccounts, {
        userID: user_id,
        caption: newCaption,
        balance: newInitialBalance
      }]);

      showNotification('Счет успешно создан', 'success');
    } catch (error) {
      if (error.response?.data?.error) {
        showNotification(error.response.data.error);
      } else {
        showNotification('Не удалось создать аккаунт');
      }
      console.error('Error creating account:', error);
    }
  };

  const handleTopUp = async (e) => {
    e.preventDefault();

    let topUpUserID = parseInt(topUp.userID);
    if (!accounts.find(account => account.userID == topUpUserID)) {
      showNotification('Аккаунт с таким ID не существует', 'warning');
      return;
    }

    let topUpOperation = parseFloat(topUp.operation);
    if (isNaN(topUpOperation)) {
      showNotification('Некорректная сумма', 'warning');
      return;
    }

    setTopUp({ userID: '', operation: '' });

    try {
      const formData = new FormData();
      formData.append('userID', topUpUserID);
      formData.append('operation', topUpOperation);

      const response = await axios.patch(`${API_URL}/api/accounts/top-up`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });

      if (response.status === 204) {
        setAccounts(prevAccounts => 
          prevAccounts.map(account => 
            account.userID === topUpUserID
              ? { ...account, balance: account.balance + topUpOperation }
              : account
          )
        );
        showNotification('Баланс успешно пополнен', 'success');
      }
    } catch (error) {
      if (error.response?.data?.error) {
        showNotification(error.response.data.error);
      } else {
        showNotification('Не удалось пополнить баланс');
      }
      console.error('Error topping up account:', error);
    }
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
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

      <Grid container spacing={3}>
        {/* Create Account Form */}
        <Grid item xs={12} md={6}>
          <Paper sx={{ p: 2 }}>
            <Typography variant="h6" gutterBottom>
              Создать новый счет
            </Typography>
            <form onSubmit={handleCreateAccount}>
              <TextField
                fullWidth
                label="Название счета"
                value={newAccount.caption}
                onChange={(e) => setNewAccount({ ...newAccount, caption: e.target.value })}
                margin="normal"
                required
              />
              <TextField
                fullWidth
                label="Начальный баланс"
                type="number"
                value={newAccount.initialBalance}
                onChange={(e) => setNewAccount({ ...newAccount, initialBalance: e.target.value })}
                margin="normal"
                required
              />
              <Button type="submit" variant="contained" color="primary" sx={{ mt: 2 }}>
                Создать
              </Button>
            </form>
          </Paper>
        </Grid>

        {/* Top Up Form */}
        <Grid item xs={12} md={6}>
          <Paper sx={{ p: 2 }}>
            <Typography variant="h6" gutterBottom>
              Пополнить счет
            </Typography>
            <form onSubmit={handleTopUp}>
              <TextField
                fullWidth
                label="ID счета"
                type="number"
                value={topUp.userID}
                onChange={(e) => setTopUp({ ...topUp, userID: e.target.value })}
                margin="normal"
                required
              />
              <TextField
                fullWidth
                label="Сумма"
                type="number"
                value={topUp.operation}
                onChange={(e) => setTopUp({ ...topUp, operation: e.target.value })}
                margin="normal"
                required
              />
              <Button type="submit" variant="contained" color="primary" sx={{ mt: 2 }}>
                Пополнить
              </Button>
            </form>
          </Paper>
        </Grid>

        {/* Accounts Table */}
        <Grid item xs={12}>
          <Paper sx={{ p: 2 }}>
            <Typography variant="h6" gutterBottom>
              Список счетов
            </Typography>
            <TableContainer>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>ID счета</TableCell>
                    <TableCell>Название счета</TableCell>
                    <TableCell align="right">Баланс</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {accounts.map((account) => (
                    <TableRow key={`${account.userID}-${account.caption}`}>
                      <TableCell>{account.userID}</TableCell>
                      <TableCell>{account.caption}</TableCell>
                      <TableCell align="right">{account.balance.toFixed(2)}</TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          </Paper>
        </Grid>
      </Grid>
    </Container>
  );
}

export default AccountsPage; 