use uni_wasm::transform;
use uni_wasm::time;

fn main() {
}

#[no_mangle]
fn update() {
    let speed = 1.0;
    let delta_t = time::get_delta_time();

    let current_position = transform::get_local_position();
    let mut x = current_position.x + speed * delta_t;
    if x > 2.0
    {
        x = -2.0
    }

    let position = transform::Vector3 {
        x: x,
        y: current_position.y,
        z: current_position.z,
    };
    transform::set_local_position(position);
}
