# lomba-apibackend
API del sistema Lomba

Para iniciar el stack debes ejecutar la siguiente línea en la carpeta raíz:

```console
docker-compose -f .\src\stack_local_compose.yml -p "lapi" up --build -d
```

Este comando crea un stack con base de datos SQL Server y API Lomba. 
El puerto de la API es el 8187, puedes probar con http://localhost:8187/swagger/index.html
Encontrarás información de la API inicial.

Los archivos de base de datos se irán a una carpeta en el disco D:
En a ubicación: D:\docker_vols\sqlserver2019