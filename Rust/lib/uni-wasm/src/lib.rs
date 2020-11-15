#[cfg(test)]
mod tests {
    #[test]
    fn it_works() {
        assert_eq!(2 + 2, 4);
    }
}

pub mod time {
    pub fn get_time() -> f32
    {
        unsafe
        {
            return time_get_time();
        }
    }

    pub fn get_delta_time() -> f32
    {
        unsafe
        {
            return time_get_delta_time();
        }
    }

    extern "C" {
        fn time_get_time() -> f32;
        fn time_get_delta_time() -> f32;
    }
}

pub mod transform {
    #[repr(C)]
    pub struct Vector3 {
        pub x: f32,
        pub y: f32,
        pub z: f32,
    }

    #[repr(C)]
    pub struct Quaternion {
        pub x: f32,
        pub y: f32,
        pub z: f32,
        pub w: f32,
    }


    pub fn set_local_position(position: Vector3)
    {
        unsafe
        {
            transform_set_local_position(position);
        }
    }

    pub fn get_local_position() -> Vector3
    {
        unsafe
        {
            return transform_get_local_position();
        }
    }

    pub fn set_local_rotation(rotation: Quaternion)
    {
        unsafe
        {
            transform_set_local_rotation(rotation);
        }
    }

    pub fn get_local_rotation() -> Quaternion
    {
        unsafe
        {
            return transform_get_local_rotation();
        }
    }

    extern "C" {
        fn transform_set_local_position(position: Vector3);
        fn transform_get_local_position() -> Vector3;
        fn transform_set_local_rotation(rotation: Quaternion);
        fn transform_get_local_rotation() -> Quaternion;
    }
}
