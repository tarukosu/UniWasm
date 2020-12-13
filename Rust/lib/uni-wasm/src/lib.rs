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
        /*
        unsafe
        {
            return transform_get_local_position();
        }
        */
        unsafe
        {
            let x = transform_get_local_position_x();
            let y = transform_get_local_position_y();
            let z = transform_get_local_position_z();
            Vector3 {
                x, y, z
            }
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

    pub fn set_local_scale(scale: Vector3)
    {
        unsafe
        {
            transform_set_local_scale(scale);
        }
    }

    pub fn get_local_scale() -> Vector3
    {
        unsafe
        {
            let x = transform_get_local_scale_x();
            let y = transform_get_local_scale_y();
            let z = transform_get_local_scale_z();
            Vector3 {
                x, y, z
            }
        }
    }

    extern "C" {
        fn transform_set_local_position(position: Vector3);
        // fn transform_get_local_position() -> Vector3;
        fn transform_get_local_position_x() -> f32;
        fn transform_get_local_position_y() -> f32;
        fn transform_get_local_position_z() -> f32;
        fn transform_set_local_rotation(rotation: Quaternion);
        fn transform_get_local_rotation() -> Quaternion;
        fn transform_set_local_scale(scale: Vector3);
        // fn transform_get_local_scale() -> Vector3;
        fn transform_get_local_scale_x() -> f32;
        fn transform_get_local_scale_y() -> f32;
        fn transform_get_local_scale_z() -> f32;
    }
}


pub mod object {
    pub fn spawn_object(resource_id: i32) -> i32
    {
        unsafe
        {
            return object_spawn_object(resource_id);
        }
    }

    extern "C" {
        fn object_spawn_object(resource_id: i32) -> i32;
    }
}
