interface CongTy {
  id?: string // Guid → string
  ten: string
  diaChi: string
  sdt: string
  email: string
  maSoThue: string
  plan: number // 1: Basic, 2: Pro
  ngayTao?: string // DateTime → ISO string
}

interface CongTyCreate {
  ten: string
  diaChi: string
  sdt: string
  email: string
  maSoThue: string
  plan: number // 1: Basic, 2: Pro
}

export type { CongTy, CongTyCreate }
