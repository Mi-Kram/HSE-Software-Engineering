#!/bin/sh

# Шаблон env.js прямо внутри скрипта
cat <<EOF > /usr/share/nginx/html/env.js.template
window.env = {
  REACT_APP_GATEWAY_SERVER: "\$WebUI_GATEWAY_SERVER"
};
EOF

# Подставляем переменные окружения
envsubst < /usr/share/nginx/html/env.js.template > /usr/share/nginx/html/env.js

# Запускаем nginx в foreground
exec nginx -g 'daemon off;'
