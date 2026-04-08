# Build, /app pg konvensjon
FROM node:20-alpine AS build
WORKDIR /app

# Installere avhengigheter
COPY package*.json ./
RUN npm install

# Kopierer inn frontend prosjektet
COPY . .

# Bygger frontenden
RUN npm run build

# "Stage" frontend til nginx serveren
FROM nginx:alpine

# Fjern default nginx-side
RUN rm -rf /usr/share/nginx/html/*

# Kopierer build inn i nginx serveren
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

CMD [ "nginx", "-g", "daemon off;" ]