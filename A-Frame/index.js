
fetch('./index.seml')
  .then(response => {
    if (!response.ok) {
      console.log(response);
      return;
    }
    return response.text();
  })
  .then(loadSeml);


function loadSeml (seml) {
  var parser = new DOMParser();
  var dom = parser.parseFromString(seml, "text/xml");

  var body = dom.getElementsByTagName('body')[0];
  var scene = document.querySelector('a-scene');
  createEntity(body, scene);
}


function createEntity (element, parent) {
  let entity;
  switch (element.tagName) {
    case 'primitive':
      var type = element.getAttribute('type');
      entity = document.createElement('a-box');
      break;
    case 'script':
      loadScript(element, parent);
      return;
    default:
      entity = document.createElement('a-entity');
      break;
  }

  if (!entity) {
    return;
  }
  console.log(entity);
  parent.appendChild(entity);

  for (var i = 0; i < element.children.length; i++) {
    var child = element.children[i];
    console.log(child.tagName);

    createEntity(child, entity);
  }
}


function generateImportObject (el) {
  var importObject = {
    wasi_snapshot_preview1: {
      proc_exit: () => { },
      fd_write: () => { },
      fd_prestat_get: () => { },
      fd_prestat_dir_name: () => { },
      environ_sizes_get: () => { },
      environ_get: () => { }
    },
    env: {
      transform_get_local_position: () => {
        return {x:1.0, y:1.0, z:1.0};
      },
      transform_get_local_position_x: () => {
        var position = el.getAttribute('position');
        return position.x;
      },
      transform_get_local_position_y: () => {
        var position = el.getAttribute('position');
        return position.y;
      },
      transform_get_local_position_z: () => {
        var position = el.getAttribute('position');
        return position.z;
      },
      transform_set_local_position: (x, y, z) => {
        el.setAttribute('position', {x, y, z});
      },
      time_get_time: () => { },
      time_get_delta_time: () => { return 0.016 },
    }
  }
  return importObject;
}

var wasm_instances = {}
var wasm_id_counter = 0;

function loadScript (element, parent) {
  var src = element.getAttribute('src');
  const importObject = generateImportObject(parent);
  fetch(src).then(response =>
    response.arrayBuffer()
  ).then(bytes =>
    WebAssembly.instantiate(bytes, importObject)
  ).then(results => {
    results.instance.exports.update();
    var wasm_id = wasm_id_counter++;
    wasm_instances[wasm_id] = results.instance;
    parent.setAttribute('wasm', "");
    parent.setAttribute('wasm-id', wasm_id);
  });
}


AFRAME.registerComponent('wasm', {
  tick: function () {
    var el = this.el;
    var wasm_id = el.getAttribute('wasm-id');
    const wasm_instance = wasm_instances[wasm_id];
    wasm_instance.exports.update();
  }
});