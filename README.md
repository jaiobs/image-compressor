# Image Compressor
Image Compressor is used to scaling/resizing the give image by height and width. It accepts byte array (base64string) / uri of the image.
It returns scaled image in the format of byte array (base64string)		

http://localhost:57829/Image/ImageResizer
Method: POST
Request:
{
    "height":500,
    "width":500,
    "imageArray":"base64string",
	"ImageURL":"URI"
}

Response:
{
	"ResizedImage":"base64string"
}

Steps to run this application:

1) Install Microsoft Visual Studio
2) Git Clone from master branch
3) Restore nuget package
4) Run API 
