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

  setValue(val) {
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
    add('num', node.data.num);
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
    this.editor.nodes.find(n => n.id == node.id).controls.get('preview').setValue(vec[0]);

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

(async () => {
  var container = document.querySelector('#rete');
  var components = [new NumComponent(), new AddComponent(), new GetPositionComponent(), new DeconstructVector3Component(), new Vector3Component()];

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

  n1.position = [0, 0];
  n2.position = [0, 200];
  n3.position = [0, 400];

  add.position = [900, 240];
  vec.position = [300, 200];
  dec.position = [600, 200];
  n4.position = [600, 400];

  new_vec.position = [1300, 200];

  editor.addNode(n1);
  editor.addNode(n2);
  editor.addNode(n3);
  editor.addNode(n4);
  
  editor.addNode(vec);
  editor.addNode(dec);
  editor.addNode(add);
  editor.addNode(new_vec);
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
    console.log(json);
    
    console.log(JSON.stringify(json, null, 2));
    const sourceCode = await CodePlugin.generate(engine, editor.toJSON());
    console.log(sourceCode);
  }, "1000");


})();
