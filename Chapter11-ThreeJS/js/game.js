var modelPath = "mesh/sephir_final.obj";
var texturePath = "texture/sephir_tex.png";

var WIDTH = 640;
var	HEIGHT = 480;

var container;

var camera, scene, renderer;

var ambient, directionalLight;

var mouseX = 0, mouseY = 0;

var loadSphere = false;

init();
animate();

function init(){
	scene = new THREE.Scene();
	SetCameraSettings();
	SetLights();
	Load();
	RenderScene();
}

function animate() {
  // This function calls itself on every frame. You can for example change
  // the objects rotation on every call to create a turntable animation.
  requestAnimationFrame( animate );
 
  // On every frame we need to calculate the new camera position
  // and have it look exactly at the center of our scene.
  controls.update();
  camera.lookAt(scene.position);
  renderer.render(scene, camera);
}

function SetCameraSettings(){
	camera = new THREE.PerspectiveCamera( 45, WIDTH / HEIGHT, 1, 2000 );
	camera.position.z = 40;
	camera.position.y = 0;
	camera.position.x = 35;
	
	controls = new THREE.TrackballControls( camera );
	controls.rotateSpeed = 5.0;
	controls.zoomSpeed = 5;
	controls.panSpeed = 2;
	controls.noZoom = false;
	controls.noPan = false;
	controls.staticMoving = true;
	controls.dynamicDampingFactor = 0.3;
	
	scene.add(camera);
}

function SetLights(){
	ambient = new THREE.AmbientLight(666666);
	scene.add(ambient);
 
	directionalLight = new THREE.DirectionalLight(0xffeedd);
	directionalLight.position.set( 0, 0, 1 ).normalize();
	scene.add(directionalLight);
}

function Load(){
	var manager = new THREE.LoadingManager();
	manager.onProgress = function ( item, loaded, total ) {
		console.log( item, loaded, total );
	};
	
	//Texture
	var texture = new THREE.Texture();
	var loader = new THREE.ImageLoader( manager );
	loader.load( texturePath, function ( image ) {
		texture.image = image;
		texture.needsUpdate = true;
	} );

	//Mesh OBJ
	var loader = new THREE.OBJLoader( manager );
	loader.load( modelPath, function ( event ) {
		var object = event;
		object.traverse(function(child){
		  if (child instanceof THREE.Mesh){
			child.material.map = texture;
		  }
		});
		object.position.y -= 10;
		scene.add(object);
	});
}

function RenderScene(){
	renderer = new THREE.WebGLRenderer();
	renderer.setClearColor(0x2F4F4F, 1);
	renderer.setSize(WIDTH, HEIGHT);
	container = document.getElementById('container');
	container.appendChild(renderer.domElement);
	renderer.render(scene, camera);
}

function MakeSphere(){
	// create the sphere's material
	var sphereMaterial = new THREE.MeshLambertMaterial(
	{
	    color: 0xCC0000
	});

	// set up the sphere vars
	var radius = 50, segments = 16, rings = 16;

	// create a new mesh with sphere geometry -
	// we will cover the sphereMaterial next!
	var sphere = new THREE.Mesh(
	   new THREE.SphereGeometry(radius, segments, rings),
	   sphereMaterial
	);

	sphere.scale.set(0.1, 0.1, 0.1);
	
	// add the sphere to the scene
	scene.add(sphere);
}

function ClearScene(){
	var obj, i;
	for ( i = scene.children.length - 1; i >= 0 ; i -- ) {
		obj = scene.children[ i ];
		if ( obj.name !== ambient && obj !== directionalLight && obj !== camera) {
			scene.remove(obj);
		}
	}
}