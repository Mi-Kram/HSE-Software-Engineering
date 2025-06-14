let socket;

export function connectToWebSocket(onMessage) {
  if (socket?.readyState === WebSocket.OPEN) return socket;

  const API_URL = window.env?.REACT_APP_GATEWAY_SERVER || '';

  const wsUrl = `${API_URL}/ws`;
  socket = new WebSocket(wsUrl);

  socket.onmessage = event => {
    onMessage(event.data);
  };

  socket.onopen = () => console.log("WebSocket подключён");
  socket.onclose = () => console.log("WebSocket отключён");

  return socket;
}
