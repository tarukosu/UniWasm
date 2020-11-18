var numSocket = new Rete.Socket('Number value');

var vector3Socket = new Rete.Socket('Vector3');
// var vector3Socket = new Rete.Socket('Number');

var VueNumControl = {
  props: ['readonly', 'emitter', 'ikey', 'getData', 'putData'],
  template: '<input type="number" :readonly="readonly" :value="value" @input="change($event)" @dblclick.stop="" @pointerdown.stop="" @pointermove.stop=""/>',
  data () {
    return {
      value: 0,
    }
  },
  methods: {
    change (e) {
      this.value = +e.target.value;
      this.update();
    },
    update () {
      if (this.ikey)
        this.putData(this.ikey, this.value)
      this.emitter.trigger('process');
    }
  },
  mounted () {
    this.value = this.getData(this.ikey);
  }
}

var VueTextControl = {
  props: ['readonly', 'emitter', 'ikey', 'getData', 'putData'],
  template: '<input type="text" :readonly="readonly" :value="value" @input="change($event)" @dblclick.stop="" @pointerdown.stop="" @pointermove.stop=""/>',
  data () {
    return {
      value: "",
    }
  },
  methods: {
    change (e) {
      this.value = e.target.value;
      this.update();
    },
    update () {
      if (this.ikey)
        this.putData(this.ikey, this.value)
      this.emitter.trigger('process');
    }
  },
  mounted () {
    this.value = this.getData(this.ikey);
  }
}

class NumControl extends Rete.Control {

  constructor(emitter, key, readonly) {
    super(key);
    this.component = VueNumControl;
    this.props = { emitter, ikey: key, readonly };
  }

  setValue (val) {
    this.vueContext.value = val;
  }
}

export class TextControl extends Rete.Control {
  constructor(emitter, key, readonly) {
    super(key);
    this.component = VueTextControl;
    this.props = { emitter, ikey: key, readonly };
  }

  setValue (val) {
    this.vueContext.value = val;
  }
}

class NumComponent extends Rete.Component {

  constructor() {
    super("Number");
  }

  builder (node) {
    var out1 = new Rete.Output('num', "Number", numSocket);

    return node.addControl(new NumControl(this.editor, 'num')).addOutput(out1);
  }

  worker (node, inputs, outputs) {
    outputs['num'] = node.data.num;
  }

  code (node, inputs, add) {
    // add('console.log("number")')
    add('num', `${node.data.num} as f32`);
    //add('num', node.data.num); // add variable with value "node.data.num"
  }
}

class AddComponent extends Rete.Component {
  constructor() {
    super("Add");
  }

  builder (node) {
    var inp1 = new Rete.Input('num', "Number", numSocket);
    var inp2 = new Rete.Input('num2', "Number2", numSocket);
    var out = new Rete.Output('numout', "Number", numSocket);

    inp1.addControl(new NumControl(this.editor, 'num'))
    inp2.addControl(new NumControl(this.editor, 'num2'))

    return node
      .addInput(inp1)
      .addInput(inp2)
      .addControl(new NumControl(this.editor, 'preview', true))
      .addOutput(out);
  }

  worker (node, inputs, outputs) {
    var n1 = inputs['num'].length ? inputs['num'][0] : node.data.num1;
    var n2 = inputs['num2'].length ? inputs['num2'][0] : node.data.num2;
    var sum = n1 + n2;
    console.log(n1);
    this.editor.nodes.find(n => n.id == node.id).controls.get('preview').setValue(sum);
    outputs['numout'] = sum;
  }

  code (node, inputs, add) {
    // add('console.log("add")')
    // add('num', inputs['num'][0]);
    //add('num', inputs['num'][0] + inputs['num1'][0]);
    add('numout', `${inputs['num'][0]}+${inputs['num2'][0]}`);
    //add('num', node.data.num); // add variable with value "node.data.num"
  }
}

class GetPositionComponent extends Rete.Component {
  constructor() {
    super("GetPosition");
  }

  builder (node) {
    var out1 = new Rete.Output('position', "Position", vector3Socket);
    return node.addOutput(out1);
  }

  worker (node, inputs, outputs) {
    outputs['position'] = [1, 2, 3];
  }
}

class Vector3Component extends Rete.Component {
  constructor() {
    super("Vector3");
  }

  builder (node) {
    var inp_x = new Rete.Input('x', "x", numSocket);
    var inp_y = new Rete.Input('y', "y", numSocket);
    var inp_z = new Rete.Input('z', "z", numSocket);

    var out = new Rete.Output('output', "Vector3", vector3Socket);
    return node
      .addInput(inp_x)
      .addInput(inp_y)
      .addInput(inp_z)
      .addControl(new TextControl(this.editor, 'preview', true))
      .addOutput(out);
  }

  worker (node, inputs, outputs) {
    //console.log(inputs['x'][0]);
    var vec = [inputs['x'][0], inputs['y'][0], inputs['z'][0]]
    this.editor.nodes.find(n => n.id == node.id).controls.get('preview').setValue(vec);
    outputs['output'] = vec;
  }

  code (node, inputs, add) {
    // add('console.log("vec")')
    // console.log(node);
    add('output', `[${inputs['x'][0]}, ${inputs['y'][0]}, ${inputs['z'][0]}]`);
  }
}

class DeconstructVector3Component extends Rete.Component {
  constructor() {
    super("Deconstruct Vector3");
  }

  builder (node) {
    var inp1 = new Rete.Input('input', "Vector3", vector3Socket);
    var out_x = new Rete.Output('x', "x", numSocket);
    var out_y = new Rete.Output('y', "y", numSocket);
    var out_z = new Rete.Output('z', "z", numSocket);
    return node
      .addInput(inp1)
      .addControl(new TextControl(this.editor, 'preview', true))
      .addOutput(out_x)
      .addOutput(out_y)
      .addOutput(out_z);
  }

  worker (node, inputs, outputs) {
    const vec = inputs['input'][0];
    this.editor.nodes.find(n => n.id == node.id).controls.get('preview').setValue(vec);

    outputs['x'] = vec[0];
    outputs['y'] = vec[1];
    outputs['z'] = vec[2];

    node.data = vec;
  }

  code (node, inputs, add) {
    add('x', `${inputs['input'][0]}[0]`);
    add('y', `${inputs['input'][0]}[1]`);
    add('z', `${inputs['input'][0]}[2]`);
  }
}

class SetLocalPositionComponent extends Rete.Component {
  constructor() {
    super("Set Local Position");
  }

  builder (node) {
    var inp1 = new Rete.Input('input', "Vector3", vector3Socket);
    return node
      .addInput(inp1)
      .addControl(new TextControl(this.editor, 'preview', true))
  }

  worker (node, inputs, outputs) {
    const vec = inputs['input'][0];
    this.editor.nodes.find(n => n.id == node.id).controls.get('preview').setValue(vec);
  }

  code (node, inputs, add) {
    add(`transform_set_local_position(${inputs['input'][0]}[0] as f32, ${inputs['input'][0]}[1] as f32, ${inputs['input'][0]}[2] as f32)`);
    //add(`transform_set_local_position_x(parseFloat(${inputs['input'][0]}[0]) as f32)`);
    //add(`transform_set_local_position_x(${inputs['input'][0]}[0] as f32)`);
    //add(`const a = transform_get_local_position_x()`);
  }
}

(async () => {
  var container = document.querySelector('#rete');
  var components = [
    new NumComponent(),
    new AddComponent(),
    new GetPositionComponent(),
    new DeconstructVector3Component(),
    new Vector3Component(),
    new SetLocalPositionComponent()
  ];

  var editor = new Rete.NodeEditor('demo@0.1.0', container);

  editor.use(ConnectionPlugin.default);
  editor.use(VueRenderPlugin.default);
  editor.use(ContextMenuPlugin.default);
  editor.use(AreaPlugin);
  editor.use(CommentPlugin.default);
  editor.use(HistoryPlugin);
  editor.use(ConnectionMasteryPlugin.default);

  var engine = new Rete.Engine('demo@0.1.0');

  components.map(c => {
    editor.register(c);
    engine.register(c);
  });

  var n1 = await components[0].createNode({ num: 1.2 });
  var n2 = await components[0].createNode({ num: -1 });
  var n3 = await components[0].createNode({ num: 3 });
  var n4 = await components[0].createNode({ num: 10 });


  var vec = await components[4].createNode();
  var dec = await components[3].createNode();
  var add = await components[1].createNode();
  var new_vec = await components[4].createNode();

  var set_local_position = await components[5].createNode();

  n1.position = [0, 0];
  n2.position = [0, 200];
  n3.position = [0, 400];

  add.position = [900, 240];
  vec.position = [300, 200];
  dec.position = [600, 200];
  n4.position = [600, 400];

  new_vec.position = [1300, 200];
  set_local_position.position = [1600, 200];

  editor.addNode(n1);
  editor.addNode(n2);
  editor.addNode(n3);
  editor.addNode(n4);

  editor.addNode(vec);
  editor.addNode(dec);
  editor.addNode(add);
  editor.addNode(new_vec);

  editor.addNode(set_local_position);
  /*
  editor.connect(n1.outputs.get('num'), add.inputs.get('num'));
  editor.connect(n2.outputs.get('num'), add.inputs.get('num2'));
  */


  editor.connect(n1.outputs.get('num'), vec.inputs.get('x'));
  editor.connect(n2.outputs.get('num'), vec.inputs.get('y'));
  editor.connect(n3.outputs.get('num'), vec.inputs.get('z'));

  editor.connect(vec.outputs.get('output'), dec.inputs.get('input'));

  editor.connect(dec.outputs.get('x'), add.inputs.get('num'));
  editor.connect(n4.outputs.get('num'), add.inputs.get('num2'));

  editor.connect(add.outputs.get('numout'), new_vec.inputs.get('x'));
  editor.connect(dec.outputs.get('y'), new_vec.inputs.get('y'));
  editor.connect(dec.outputs.get('z'), new_vec.inputs.get('z'));

  editor.connect(new_vec.outputs.get('output'), set_local_position.inputs.get('input'));


  editor.on('process nodecreated noderemoved connectioncreated connectionremoved', async () => {
    console.log('process');
    await engine.abort();
    await engine.process(editor.toJSON());
  });

  editor.view.resize();
  AreaPlugin.zoomAt(editor);
  editor.trigger('process');


  window.setTimeout(async () => {

    const json = editor.toJSON()
    //console.log(json);
    //console.log(JSON.stringify(json, null, 2));
    const sourceCode = await CodePlugin.generate(engine, editor.toJSON());
    //console.log(sourceCode);
  }, "1000");


  async function generateCode () {
    const code = await CodePlugin.generate(engine, editor.toJSON());
    const declare_code = `
    export declare function transform_set_local_position(x: f32, y: f32, z: f32): void;
    export declare function transform_get_local_position_x(): f32;
    `

    const update_func = `
    export function update(): void {
    `
      + code +
      `}`
    let full_code = declare_code + update_func;
    // full_code = code;
    console.log(full_code);

    const { text, binary, stdout, stderr } = g_asc.compileString(full_code, {
      optimizeLevel: 3,
      runtime: "none"
    });

    console.log(stdout.toString());
    console.log(stderr.toString());
    console.log(g_asc);
    console.log(text);
    console.log(binary);
    //var content = 'あいうえお';
    //var blob = new Blob([ content ], { "type" : "text/plain" });

    var blob = new Blob([binary.buffer], { type: "application/wasm" });
    console.log(blob);
    var objectUrl = URL.createObjectURL(blob);
    console.log(objectUrl);
    // window.open(objectUrl);

    const a = document.createElement("a");
    document.body.appendChild(a);
    a.download = 'test.wasm';
    a.href = objectUrl;
    a.click();
    a.remove();
    URL.revokeObjectURL(objectUrl);
    //console.log(`>>> TEXT >>>\n${text}`);
  }

  document.getElementById("download").onclick = generateCode;

  var g_asc;

  require(["https://cdn.jsdelivr.net/npm/assemblyscript@latest/dist/sdk.js"], ({ asc }) => {
    asc.ready.then(() => {
      console.log("Running simple example...");
      // asc = _asc;
      g_asc = asc;
      //simpleExample(asc);
      generateCode();
      console.log("\nRunning extended example...");
      // extendedExample(asc);
    });
  });

  const SOURCE_CODE = `export function test(): void {}`;

  // This uses `asc.compileString`, a convenience API useful if all one wants to
  // do is to quickly compile a single source string to WebAssembly.
  async function simpleExample (asc) {
    const code = await CodePlugin.generate(engine, editor.toJSON());
    const { text, binary } = asc.compileString(code, {
      optimizeLevel: 3,
      runtime: "none"
    });
    console.log(`>>> TEXT >>>\n${text}`);
    console.log(`>>> BINARY >>>\n${binary.length} bytes`);
  }

  /*
  // The full API works very much like asc on the command line, with additional
  // environment bindings being provided to access the (virtual) file system.
  function extendedExample(asc) {
    const stdout = asc.createMemoryStream();
    const stderr = asc.createMemoryStream();
    asc.main([
      "module.ts",
      "-O3",
      "--runtime", "none",
      "--binaryFile", "module.wasm",
      "--textFile", "module.wat",
      "--sourceMap"
    ], {
      stdout,
      stderr,
      readFile(name, baseDir) {
        return name === "module.ts" ? SOURCE_CODE : null;
      },
      writeFile(name, data, baseDir) {
        console.log(`>>> WRITE:${name} >>>\n${data.length}`);
      },
      listFiles(dirname, baseDir) {
        return [];
      }
    }, err => {
      console.log(`>>> STDOUT >>>\n${stdout.toString()}`);
      console.log(`>>> STDERR >>>\n${stderr.toString()}`);
      if (err) {
        console.log(">>> THROWN >>>");
        console.log(err);
      }
    });
  }
  */
})();
