FROM node:18-alpine AS build

WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

FROM nginx:alpine

COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/build /usr/share/nginx/html

# Просто добавим шаблон с переменной
RUN echo 'window.env = { REACT_APP_GATEWAY_SERVER: "$WebUI_GATEWAY_SERVER" };' > /usr/share/nginx/html/env.template.js

EXPOSE 80
