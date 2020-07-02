use std::fmt;

#[derive(PartialEq, Eq)]
#[derive(Debug)]
pub enum Direction{
    N, E, S, W, None
}
pub struct Dron{
    x_position :i32,
    y_position :i32,
    dir: Direction,
}
impl Dron{
    pub fn new() -> Dron {
        Dron{
            x_position : 0,
            y_position : 0,
            dir : Direction::None
        }
    }
}
impl fmt::Display for Direction {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
        write!(f, "{:?}", self)
    }
}

fn main() {
    println!("Yellowstone’s park control forest");

    let test1 = String::from("5 5 3 3 E L 3 3 E MMRMMRMRRM 1 2 N LM LM LM LM ML ML ML ML MM");

    if test1.len() <= 3 {
        return;
    }

    let area: Vec<i32> = test1[0..3]
        .split_whitespace()
        .map(|s| s.parse().expect("parse error"))
    .collect();

    println!("Dron Area defined: {}x{}", area[0], area[1]);
    let mut result = String::new();
    let mut i = 2;
    let movements : Vec<&str> = test1.split_whitespace().collect();
    let mut dron: Dron = Dron::new();
    while i < movements.len() {
        let curr = movements[i].trim().to_uppercase();
        println!("Evaluate {} -- {},{}", curr, i, movements.len());
        if curr.parse::<i32>().is_ok() && (i + 2) <= movements.len(){
            if dron.x_position > 0 && dron.y_position > 0 {
                result += &format!("{} {} {} ", dron.x_position, dron.y_position, dron.dir);
            }
            println!("Evaluate {} {} {}", movements[i],movements[i+1],movements[i+2]);
            dron = Dron::new();
            dron.x_position = curr.parse::<i32>().unwrap();
            dron.y_position = movements[i+1].trim().parse::<i32>().unwrap();
            dron.dir = get_direction(&movements[i+2].trim());
            i = i + 3;
            println!("Set initial position {} {} {} => {}", dron.x_position, dron.y_position, dron.dir, i);
        }else if curr.starts_with("M") || curr.starts_with("L") || curr.starts_with("R"){
            for elem in curr.chars() {
                if elem == 'L' || elem == 'R' {
                    dron.dir = movement(&elem.to_string(), dron.dir);
                }else if elem == 'M' {
                    if dron.dir == Direction::N {
                        dron.y_position = dron.y_position + 1;
                    }else if dron.dir == Direction::S {
                        dron.y_position = dron.y_position - 1;
                    }else if dron.dir == Direction::E {
                        dron.x_position = dron.x_position + 1;
                    }else if dron.dir == Direction::W {
                        dron.x_position = dron.x_position - 1;
                    }
                }
            }
            i = i + 1;
            if i == movements.len() {
                result += &format!("{} {} {} ", dron.x_position, dron.y_position, dron.dir);
            }
        }else{
            println!("Invalid command");
        }
    }
    println!("Output movements: {}", result.trim());
    assert_eq!(result.trim(), "3 3 N 5 1 E 1 4 N");
}
fn get_direction(data: &str) -> Direction {
    println!("get_direction {}", data);
    if data == "N"{
        return Direction::N;
    }else if data == "S" {
        return Direction::S;
    }else if data == "E" {
        return Direction::E;
    }else if data == "W" {
        return Direction::W;
    }
    return Direction::None;
}
fn movement(data: &str, dir: Direction) -> Direction{
    println!("movement {} direction {}", data, dir);
    if data == "L" {
        if dir == Direction::N {
            return Direction::W;
        }else if dir == Direction::S {
            return Direction::E;
        }else if dir == Direction::E {
            return Direction::N;
        }else if dir == Direction::W {
            return Direction::S;
        }
    }else{
        if dir == Direction::N {
            return Direction::E;
        }else if dir == Direction::S {
            return Direction::W;
        }else if dir == Direction::E {
            return Direction::S;
        }else if dir == Direction::W {
            return Direction::N;
        }
    }
    return Direction::None;
}