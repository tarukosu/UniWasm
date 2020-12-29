use once_cell::sync::Lazy;
use clap::Clap;

use uni_wasm::debug;
use uni_wasm::transform::Transform;
use uni_wasm::Vector3;

#[derive(Clap)]
struct Opts {
    #[clap(short, long)]
    position: Vec<f32>,

    #[clap(short, long)]
    world: bool,

    #[clap(short, long, conflicts_with("world"))]
    local: bool,
}

enum Coordinate {
    Local,
    World,
}

struct Config {
    coordinate: Coordinate,
    position: Option<Vector3>,
}

static CONFIG: Lazy<Config> = Lazy::new(|| parse_args());

fn parse_args() -> Config {
    let opts = Opts::parse();

    let position = if let [x, y, z] = opts.position[..] {
        Some(Vector3::new(x, y, z))
    } else {
        None
    };

    let coordinate = if opts.world {
        Coordinate::Local
    } else {
        Coordinate::World
    };

    Config {
        coordinate: coordinate,
        position: position,
    }
}

#[no_mangle]
fn update() {
    let transform = Transform::myself();

    if let Some(position) = &CONFIG.position {
        match &CONFIG.coordinate {
            Coordinate::Local => transform.set_local_position(*position),
            Coordinate::World => transform.set_world_position(*position),
        }
    }
}
