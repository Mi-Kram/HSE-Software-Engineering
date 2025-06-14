import React, { useEffect, useState } from "react";
import ServerError from "./ServerError";
import axios from 'axios';
import { Typography } from '@mui/material';

const HealthCheckGuard = ({ children }) => {
    const [isServerAvailable, setIsServerAvailable] = useState(true);
    const [hasCheckedInitially, setHasCheckedInitially] = useState(false);
  
    useEffect(() => {
      const checkServerHealth = async () => {
        try {
          if (!window.env?.REACT_APP_GATEWAY_SERVER) {
            console.error("Gateway server URL is not defined");
            setIsServerAvailable(false);
            return;
          }
  
          await axios.get(`${window.env.REACT_APP_GATEWAY_SERVER}/api/health`, {
            validateStatus: (status) => status === 204 || (status >= 200 && status < 300)
          });
          setIsServerAvailable(true);
        } catch (error) {
          console.error("Server health check failed:", error);
          setIsServerAvailable(false);
        } finally {
          setHasCheckedInitially(true);
        }
      };
  
      // Первая проверка
      checkServerHealth();
  
      // Повторная проверка каждые 5 секунд
      const interval = setInterval(checkServerHealth, 5000);
      return () => clearInterval(interval);
    }, []);
  
    if (!hasCheckedInitially) {
      return (
        <div
          style={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            height: "100vh",
          }}
        >
          <Typography variant="h5">Загрузка...</Typography>
        </div>
      );
    }
  
    if (!isServerAvailable) {
      return <ServerError />;
    }
  
    return children;
  };

export default HealthCheckGuard;
