use uni_wasm::transform;
use uni_wasm::time;

fn main() {
}

#[no_mangle]
fn update() {
    let position = transform::Vector3 {
        x: 1.0,
        y: 2.0,
        z: 3.0
    };
    transform::set_local_position(0, position);
}
