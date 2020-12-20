use uni_wasm::transform;
use uni_wasm::object;
use uni_wasm::physics;

fn main() {
}

#[no_mangle]
unsafe fn on_use() {
    let object_id = object::spawn_object(1);

    let gun_position = transform::get_world_position(0);
    let forward = transform::get_world_forward(object_id);

    let offset = 0.5;
    let position = transform::Vector3 {
        x: gun_position.x + offset * forward.x,
        y: gun_position.y + offset * forward.y,
        z: gun_position.z + offset * forward.z,
    };

    transform::set_world_position(object_id, position);

    let rotation = transform::get_world_rotation(0);
    transform::set_world_rotation(object_id, rotation);

    let speed = 4.0;

    let velocity = transform::Vector3 {
        x: forward.x * speed,
        y: forward.y * speed,
        z: forward.z * speed,
    };

    physics::set_world_velocity(object_id, velocity);
}
