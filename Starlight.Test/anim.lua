function __swipe_down(x, y)
  return (10 * math.cos((x / columns(y) + _time)) * math.sin(_time)) * _delta
end

function __anim2(x, y)
  
end

function pixel(x, y)
  local width = columns(y)
  
  local cx1 = math.sin(_time / 4) * width / 6 + width / 2
  local cy1 = math.sin(_time / 8) * _rows / 6 + _rows / 2
  local cx2 = math.cos(_time / 6) * width / 6 + width / 2
  local cy2 = math.cos(_time) * _rows / 6 + _rows / 2
  
  local dx = ((x - cx1) ^ 2) // 1
  local dy = ((y - cy1) ^ 2) // 1
  
  local dx2 = ((x - cx2) ^ 2) // 1
  local dy2 = ((y - cy2) ^ 2) // 1
  
  return 10 * ((((math.sqrt(dx + dy) // 1) ~ (math.sqrt(dx2 + dy2) // 1)) >> 4) & 1) * _delta
end