use uni_wasm::Vector3;
use uni_wasm::transform::Transform;

#[no_mangle]
fn update() {
    let position = Vector3::new(0.1, 0.2, 0.3);
    let transform = Transform::myself();
    transform.set_local_position(position);
}
