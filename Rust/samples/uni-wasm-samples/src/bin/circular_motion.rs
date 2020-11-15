use uni_wasm::transform;
use uni_wasm::time;

fn main() {
}

#[no_mangle]
fn update() {
    let t = time::get_time();
    let fract = t.fract();

    let theta = fract * 2.0 * std::f32::consts::PI;
    let x = theta.cos();
    let y = theta.sin();

    let position = transform::Vector3 {
        x: x,
        y: y,
        z: 0.0,
    };
    transform::set_local_position(position);
}
