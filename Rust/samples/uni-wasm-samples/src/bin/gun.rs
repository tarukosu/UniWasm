use element::Element;
use transform::Transform;
use uni_wasm::element;
use uni_wasm::physics;
use uni_wasm::{Vector3, transform};

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
    let transform = Transform::myself();
    let gun_position = transform.get_world_position();
    let gun_rotation = transform.get_world_rotation();

    // spawn bullet
    let result = element::spawn_object_by_id("bullet");
    if result.is_err() {
        return;
    }
    let bullet_element = result.unwrap();

    // set position and rotation
    let offset = 0.5;
    let forward = transform.forward();
    let bullet_position = gun_position + offset * forward;

    let bullet_transform = bullet_element.transform();
    bullet_transform.set_world_position(bullet_position);
    bullet_transform.set_world_rotation(gun_rotation);

    // set velocity
    let speed = 4.0;
    let velocity = speed * forward;
    bullet_element.physics().set_world_velocity(velocity);
}
