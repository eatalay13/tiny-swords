extends Sprite2D

@export var gold_mine_active: Texture2D
@export var gold_mine_in_active: Texture2D
var gold_scene: PackedScene = preload("res://scenes/prefabs/gold.tscn")

var _is_active: bool = false

var is_active: bool = false:
	get:
		return _is_active
	set (value):
		_is_active = value
		if(value):
			texture = gold_mine_active
			$GSpawnTimer.start()
		else:
			texture = gold_mine_in_active
			$GSpawnTimer.stop()

var _goldSpawnStartPosition: int = -72
var _goldSpawnCounter:int = 0
var _goldSpawnRateMax:int = 8

var is_full: bool = false : 
	get:
		return _goldSpawnCounter >= _goldSpawnRateMax

func _ready() -> void:
	is_active = true

func _full_capacity() -> void:
	is_active = false
	_goldSpawnCounter = _goldSpawnRateMax

func _on_g_spawn_timer_timeout() -> void:
	if !is_active: return
	
	var gold:Area2D = gold_scene.instantiate()
	
	get_parent().add_child(gold)
	
	gold.global_position = global_position + Vector2(_goldSpawnStartPosition + (_goldSpawnCounter * 32), 80 if _goldSpawnCounter>5  else 48)
	
	_goldSpawnCounter += 1
	
	if is_full:
		_full_capacity()
