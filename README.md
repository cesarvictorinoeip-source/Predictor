# Predictor
# 📈 Predictor de Tendencias de Activos

Aplicación web desarrollada con **ASP.NET Core MVC** que predice la tendencia del precio de un activo (acciones o criptomonedas) a partir de su historial de los últimos 20 días, usando tres modelos de análisis distintos.

> Proyecto académico desarrollado en el Instituto Tecnológico de Las Américas (ITLA).

---

## ✨ Funcionalidades

- Registro del historial de precios de un activo (20 valores, del más reciente al más antiguo).
- Selección del **modo de predicción** desde la sección *Modos*. El modo elegido se mantiene guardado en memoria mientras la aplicación está en ejecución.
- Cálculo de la predicción y visualización del resultado según el modo seleccionado.

---

## 🧮 Modos de predicción

### 1. Media Móvil Simple (SMA) Crossover *(modo por defecto)*
Compara el promedio de los **5 precios más recientes** (SMA corta) con el promedio de los **20 precios** (SMA larga).
- SMA corta **mayor** que la larga → tendencia **alcista** 📈
- SMA corta **menor** que la larga → tendencia **bajista** 📉

**Resultado:** valor de la SMA corta, valor de la SMA larga y la tendencia.

### 2. Regresión Lineal
Ajusta una recta de mínimos cuadrados (`y = mx + b`) sobre el precio en función del tiempo, calculando la pendiente `m` y el intercepto `b`.

**Resultado:** el precio estimado para el **día 21** y si la tendencia es alcista o bajista según la pendiente.

### 3. Momentum (ROC — Rate of Change)
Mide la velocidad de cambio del precio respecto al valor de **5 días atrás**:

```
ROC = (Precio actual / Precio de hace 5 días - 1) × 100
```

**Resultado:** la lista de precios con su ROC en cada punto (los primeros 5 aparecen como *n/a* por no tener suficientes datos previos).

---

## 🛠️ Tecnologías

| Área | Tecnologías |
|---|---|
| Backend | C#, ASP.NET Core MVC |
| Arquitectura | Separación en capa web y capa de lógica de negocio |
| Validaciones | ViewModels con Data Annotations |
| Transferencia de datos | DTOs |
| Persistencia del modo | Servicio Singleton (en memoria) |
| Interfaz | Bootstrap |

---

## 🏗️ Estructura

- **Capa Web (MVC)** → Controladores, vistas y ViewModels con sus validaciones.
- **Capa de lógica de negocio** → Servicios con los cálculos de cada modo de predicción y DTOs para devolver los resultados.

---

## 🚀 Cómo ejecutarlo

### Requisitos
- [.NET SDK](https://dotnet.microsoft.com/download) (versión del proyecto)
- Visual Studio 2022 (recomendado)

### Pasos

1. **Clona el repositorio**
   ```bash
   git clone https://github.com/cesarvictorinoeip-source/AssetTrendPredictor.git
   ```

2. **Abre la solución** en Visual Studio y ejecuta el proyecto web.

No necesita base de datos ni configuración adicional.

### Ejemplo de datos de entrada
```
2025-05-20, 152.10
2025-05-19, 150.35
2025-05-18, 149.80
...
```
*(20 filas en total, del más reciente al más antiguo)*

---

## 👨‍💻 Autor

**César Tomás Victorino**
Estudiante de Desarrollo de Software — ITLA

- GitHub: [@cesarvictorinoeip-source](https://github.com/cesarvictorinoeip-source)
