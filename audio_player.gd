extends AudioStreamPlayer

const level_music = preload("res://Assets/Sound/Music/Samba Isobel.mp3")

func _play_music(music: AudioStream, volume = -25):
	if stream == music:
		return
	
	stream = music
	volume_db = volume
	play()
	
func _ready():
	_play_music(level_music)
