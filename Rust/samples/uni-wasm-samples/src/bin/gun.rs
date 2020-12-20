use uni_wasm::transform;
use uni_wasm::element;
use uni_wasm::physics;

fn main() {
    let bullet_index = element::get_resource_index_by_id("123");
    print!("{}", bullet_index);
}

/*
#[no_mangle]
unsafe fn update() {
    let bullet_index = element::get_resource_index_by_id("123ab");
    print!("{}", bullet_index);
}
*/

#[no_mangle]
unsafe fn on_use() {
    let bullet_index = element::get_resource_index_by_id("bullet");
    let object_id = element::spawn_object(bullet_index);

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
