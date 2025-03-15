# Med Rostering

This is just a simple approach to resolve the med/nurse rostering problem.

### Yaml document
All this request things can be setup in `configuration.yaml` file like the follow example:

```yaml
mes: mayo # "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"
cantidad_guardias_a_generar: 20
dias_con_dos_medicos: ["martes", "jueves"] # "lunes", "martes", "miercoles", "jueves", "viernes", "sabado", "domingo"
medicos:
  - nombre: Juan
    peticiones:
    dias_sin_guardia: [] # "lunes", "martes", "miercoles", "jueves", "viernes", "sabado", "domingo"
    vacaciones:
        inicio: 0
        fin: 0
    dias_peticion: [1,4,7,8,28]
  ...
```
Each doctor can have the follow restrictions for the month:

- dias_peticion: numeric array that represent the days the doctor cannot work.
- dias_sin_guardia: days (in spanish) that the doctor cannot work every week.
- vacaciones: start and end of vacation period

The others parameters means:

- mes: current month to process
- cantidad_guardias_a_generar: quantity of shift to generate
- dias_con_dos_medicos: days in witch is necessary to have two doctors.


### How to run

- Console: Just modify the "configuration.yaml" document that is in the Rostering.Console project and execute it the project.
- Web: Just execute the web project and upload the "configuration.yaml" document.