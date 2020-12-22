use uni_wasm::time;
use uni_wasm::transform;

fn main() {}

#[no_mangle]
fn update() {
    let speed = 1.0;
    let delta_t = time::get_delta_time();

    let current_position = transform::get_local_position(0);
    let mut x = current_position.x + speed * delta_t;
    if x > 2.0 {
        x = -2.0
    }

    let position = transform::Vector3::new(x, current_position.y, current_position.z);
    transform::set_local_position(0, position);
}
