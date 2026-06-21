# Aritmética Veloz 🎯

Juego educativo 2D en Unity dirigido a niños de 5 a 12 años. Combina mecánicas de tiro (estilo Angry Birds) con matemáticas básicas.

## 🎮 Características

- **Mecánica de Tiro**: Apunta y dispara flechas hacia objetivos
- **Problemas Matemáticos**: Sumas, restas y más operaciones
- **Objetivos Interactivos**: Globos, cajas y tablones con números
- **Sistema de Puntuación**: Gana puntos resolviendo operaciones correctamente
- **Retroalimentación Visual**: Efectos y animaciones para mantener la atención

## 📁 Estructura del Proyecto

```
Assets/
├── Scripts/
│   ├── GameManager.cs
│   ├── ArcherController.cs
│   ├── ArrowProjectile.cs
│   ├── TargetObject.cs
│   ├── UIManager.cs
│   └── QuestionData.cs
├── Scenes/
│   └── GameScene.unity
├── Prefabs/
│   ├── Arrow.prefab
│   └── Target.prefab
├── UI/
└── Resources/
```

## 🚀 Cómo Empezar

1. **Clona este repositorio**:
   ```bash
   git clone https://github.com/Dieguisto-1318/Aritmetica-Veloz.git
   cd Aritmetica-Veloz
   ```

2. **Abre en Unity**:
   - Abre Unity Hub
   - Click en "Add Project from disk"
   - Selecciona la carpeta `Aritmetica-Veloz`

3. **Carga la escena**:
   - En el proyecto, ve a `Assets/Scenes/GameScene.unity`
   - Haz doble click para abrirla

4. **Presiona Play** para probar el juego

## 📚 Scripts Principales

- **GameManager.cs**: Gestiona la lógica del juego, preguntas y puntuación
- **ArcherController.cs**: Controla el arco, aiming y disparo de flechas
- **ArrowProjectile.cs**: Física de la flecha y detección de colisiones
- **TargetObject.cs**: Representa cada objetivo con su número
- **UIManager.cs**: Interfaz del juego (pregunta, puntos, etc.)
- **QuestionData.cs**: Almacena y genera preguntas matemáticas

## 🎯 Próximos Pasos

- [ ] Crear sprites personalizados
- [ ] Añadir sonidos y efectos
- [ ] Implementar diferentes niveles de dificultad
- [ ] Agregar poder-ups

## 👨‍💻 Autor

Dieguisto-1318

## 📄 Licencia

MIT License
