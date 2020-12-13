use uni_wasm::transform;
use uni_wasm::object;
use uni_wasm::physics;

fn main() {
}

#[no_mangle]
fn update() {
    // object::spawn_object(1);
}

#[no_mangle]
unsafe fn on_touch_start() {
    let object_id = object::spawn_object(1);
    let velocity = transform::Vector3 {
        x: 0.0,
        y: 0.0,
        z: 1.0,
    };

    let position = transform::Vector3 {
        x: 0.0,
        y: 0.0,
        z: 0.2,
    };

    transform::set_local_position(object_id, position);
    physics::set_velocity(object_id, velocity);
}
