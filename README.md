# lomba-apibackend
API del sistema Lomba

Para iniciar el stack debes ejecutar la siguiente línea en la carpeta raíz:

```console
docker-compose -f stack_local_compose.yml -p "lapi" up --build -d
```

Este comando crea un stack Docker Compose con base de datos SQL Server y API Lomba. 