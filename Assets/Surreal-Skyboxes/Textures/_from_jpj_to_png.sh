for f in *.jpg; do 
	ffmpeg -i "$f" "$(basename $f .jpg)".png; 
	rm "$f";
done
