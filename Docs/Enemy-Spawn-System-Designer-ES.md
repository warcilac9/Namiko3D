# Sistema de Oleadas de Enemigos (Guia Corta para Diseno)

## Que controla este sistema
- enemiesToSpawn: cantidad total de enemigos de la oleada.
- maxEnemies: cantidad maxima de enemigos vivos al mismo tiempo.

Ejemplo:
- Si enemiesToSpawn = 10 y maxEnemies = 3, primero aparecen 3.
- Cada vez que derrotas 1, aparece 1 nuevo.
- Termina cuando ya salieron y murieron los 10.

## Configuracion rapida en Inspector

### 1) En el trigger de horda
Componente: hordeEnemyTrigger

- triggerEffect: asignar el ScriptableObject de spawn.
- enemiesToSpawn: total de la oleada.
- maxEnemies: vivos simultaneos.
- triggerObj: objeto del trigger que se apaga al activarse.

### 2) En el listener del evento
Componente: EnemySpawnListener

- gameEvent: debe ser el mismo ScriptableObject.
- response: conectar a SpawnEnemies.spawnEnemies.

### 3) En el spawner
Componente: SpawnEnemies

- enemyPoolManager: referencia al EnemyPoolManager.
- spawnPoint (opcional): punto exacto de aparicion.

### 4) En el pool de enemigos
Componente: EnemyPoolManager

- Prefab: prefab del enemigo.
- origin: transform de origen para spawneo.
- bulletList: lista del pool (si ya la usas en inspector).

## Flujo en juego (resumen)
1. El jugador entra al trigger.
2. Se lanza el evento de spawn.
3. Aparecen enemigos hasta el limite maxEnemies.
4. Al derrotar uno, aparece el siguiente pendiente.
5. Cuando no quedan pendientes, la oleada finaliza.

## Checklist de prueba (1 minuto)
1. Configura enemiesToSpawn = 8 y maxEnemies = 2.
2. Entra al trigger.
3. Verifica que solo haya 2 vivos.
4. Derrota 1 y confirma que aparece 1 nuevo.
5. Repite hasta terminar la oleada.
6. Confirma que no aparecen enemigos extra.

## Errores comunes
- No pasa nada al entrar al trigger:
  - Revisar que triggerEffect y gameEvent apunten al mismo ScriptableObject.
- No se generan enemigos:
  - Revisar referencia a EnemyPoolManager y Prefab.
- Se generan demasiados:
  - Revisar maxEnemies en hordeEnemyTrigger.
