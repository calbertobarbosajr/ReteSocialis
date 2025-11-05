# Etapa 1 — Build do Angular
FROM node:22 AS build
WORKDIR /app

# Copia apenas arquivos de dependência primeiro
COPY package*.json ./

# Instala todas as dependências (inclusive dev)
RUN npm ci

# Copia o restante do código
COPY . .

# Build usando o CLI local (garante compatibilidade de versão)
RUN npx ng build --configuration production

# Etapa 2 — Servindo com Nginx
FROM nginx:stable-alpine
COPY --from=build /app/dist/ReteSocialis.Web /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
