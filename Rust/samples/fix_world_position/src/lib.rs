use once_cell::sync::Lazy;

use uni_wasm::debug;
use uni_wasm::transform::Transform;
use uni_wasm::Vector3;

use clap::Clap;

#[derive(Clap)]
struct Opts {
    #[clap(short, long)]
    position: Vec<f32>,

    #[clap(short, long)]
    world: bool,

    #[clap(short, long, conflicts_with("world"))]
    local: bool,
}



// static OPTS: Lazy<Mutex<Opts>> = Lazy::new(|| Mutex::new(parse_args()));
static OPTS: Lazy<Opts> = Lazy::new(|| parse_args());

fn parse_args() -> Opts {
    Opts::parse()
}

#[no_mangle]
fn start() {
    /*
    for p in opts.position {
        let message = format!("position {}", p);
        debug::log_info(&message);
    }
    */
    // let opts = OPTS;
    for p in &OPTS.position {
        let message = format!("position {}", p);
        debug::log_info(&message);
    }
    /*
    let opts = OPTS.lock().unwrap();
    for p in &opts.position {
        let message = format!("position {}", p);
        debug::log_info(&message);
    }
    */
}

#[no_mangle]
fn update() {
    let position = Vector3::new(0.1, 0.2, 0.3);
    let transform = Transform::myself();

    for p in &OPTS.position {
        let message = format!("position {}", p);
        debug::log_info(&message);
    }
    /*
    if matches.opt_present("w") {
        transform.set_world_position(position);
    } else {
        transform.set_local_position(position);
    }
    */
}
