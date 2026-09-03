**UNIVERSIDAD PRIVADA DOMINGO SAVIO**

![](media/image1.png){width="2.5625in" height="2.7083333333333335in"}

**INGENIERIA INDUSTRIAL**

# 

# SISTEMA DE INVENTARIO DE MATERIA PRIMA

**Materia:** Programación Numérica y Aplicaciones

**Docente:** Lic. Andrés Grover Albino Chambi

**Grupo:** 1

**integrantes:**

- Jazmin Viera Gonzales Huanca

- William Condori Mamani

**Gestión:** 2026

**[\]{.underline}**

**[SISTEMA DE CONTROL DE INVENTARIO DE MATERIA PRIMA]{.underline}**

**1.DESCRIPCIÓN DEL PROYECTO**

Este sistema tiene como objetivo automatizar el control y registro del inventario de materias primas en un entorno industrial. La solución reemplaza los registros manuales en papel, permitiendo al personal registrar, consultar, actualizar y listar existencias de forma rápida y confiable, reduciendo errores humanos y facilitando la toma de decisiones.

**2.OBJETIVOS ESPECÍFICOS**

1.  Digitalizar el registro de materias primas guardando: código, nombre, unidad de medida, stock actual y stock mínimo.

2.  Implementar persistencia permanente mediante archivos de texto plano(.txt) para conservar la información al cerrar el programa.

3.  Automatizar el control de entradas y salidas, así como detectar automáticamente niveles bajos de existencias.

4.  Generar alertas visuales y reportes completos ordenados para revisión o impresión.

**3.TECNOLOGÍAS UTILIZADAS**

- **Lenguaje:** C# 7.3 sobre .NET Framework

- **Entorno:** Visual Studio 2019 / 2022 -- Aplicación de Consola

- **Almacenamiento:** Archivos de texto plano con separador  \| 

- **Codificación:** UTF‑8 para mostrar correctamente tildes, eñes y caracteres especiales

- **Estructura:** Arreglos paralelos organizados por campos y métodos independientes por cada función

**4.FUNCIONALIDADES DEL SISTEMA**

\_El menú principal cuenta con estas operaciones:

1.  **Registrar nueva materia prima** → Valida que el código no se repita, que no queden campos vacíos y que cantidades sean positivas.

2.  **Listar todo el inventario** → Muestra tabla completa con estado resaltado en colores.

3.  **Actualizar existencias** → Permite sumar entradas o restar salidas; impide retirar más de lo disponible.

4.  **Listar solo artículos bajo mínimo** → Muestra alertas de reorden.

5.  **Buscar por código** → Localiza y muestra todos los datos de un elemento exacto.

6.  **Generar reporte completo** → Crea automáticamente Reporte_Inventario.txt con fecha, hora, totales y estado.

7.  **Salir del sistema** → Guarda automáticamente todos los cambios antes de cerrar.

**5.ESTRUCTURA DE DATOS Y ALMACENAMIENTO**

- Se usan arreglos paralelos: códigos, nombres, unidades, stock actual y stock mínimo.

- Datos se guardan en materias_primas.txt  en el formato:

- CODIGO\|NOMBRE\|UNIDAD\|STOCK_ACTUAL\|STOCK_MINIMO 

- Al iniciar carga los archivos guardados; al terminar guarda todo nuevamente.

**6.VALIDACIONES Y PROTECCIONES**

- Rechaza espacios vacíos

- No permite códigos duplicados

- Solo acepta números mayores o iguales a cero

- Impide retiros mayores al stock existente

- Ignora letras donde se requieren cantidades numéricas

- Muestra mensajes claros de éxito, advertencia o error con colores

**7.INSTRUCCIONES DE INSTALACIÓN Y EJECUCIÓN**

1)  Crea un proyecto de Aplicación de Consola (.NET Framework) en Visual Studio.

2)  Reemplaza el contenido predeterminado de  Program.cs  con el código entregado.

3)  El programa ya trae configurado:

4)  Console.OutputEncoding = Encoding.UTF8;  para que las letras con tildes salgan bien.

5)  Presiona F5 para compilar y ejecutar.

6)  Se crearán solos dos archivos en tu carpeta del proyecto:

materias_primas.txt → guarda tu base de datos

Reporte_Inventario.txt → se genera cuando pides imprimir el listado

**8.CONCLUSIÓN**

El proyecto resuelve completamente el problema planteado: digitalizar el inventario con persistencia propia, controles seguros, alertas de utilidad práctica y documentos listos para entregar. Combina todos los temas vistos: arreglos, descomposición en procesos independientes, entradas seguras y manejo de archivos externos.

**https://github.com/jazzvg02-bot/Jazmin_Viera_Gonzales.git**
**https://www.youtube.com/watch?v=JNzsqqmXNXE**
